// This code contains 3D SYSTEMS Confidential Information and is disclosed to you
// under a form of 3D SYSTEMS software license agreement provided separately to you.
//
// Notice
// 3D SYSTEMS and its licensors retain all intellectual property and
// proprietary rights in and to this software and related documentation and
// any modifications thereto. Any use, reproduction, disclosure, or
// distribution of this software and related documentation without an express
// license agreement from 3D SYSTEMS is strictly prohibited.
//
// ALL 3D SYSTEMS DESIGN SPECIFICATIONS, CODE ARE PROVIDED "AS IS.". 3D SYSTEMS MAKES
// NO WARRANTIES, EXPRESSED, IMPLIED, STATUTORY, OR OTHERWISE WITH RESPECT TO
// THE MATERIALS, AND EXPRESSLY DISCLAIMS ALL IMPLIED WARRANTIES OF NONINFRINGEMENT,
// MERCHANTABILITY, AND FITNESS FOR A PARTICULAR PURPOSE.
//
// Information and code furnished is believed to be accurate and reliable.
// However, 3D SYSTEMS assumes no responsibility for the consequences of use of such
// information or for any infringement of patents or other rights of third parties that may
// result from its use. No license is granted by implication or otherwise under any patent
// or patent rights of 3D SYSTEMS. Details are subject to change without notice.
// This code supersedes and replaces all information previously supplied.
// 3D SYSTEMS products are not authorized for use as critical
// components in life support devices or systems without express written approval of
// 3D SYSTEMS.
//
// Copyright (c) 2021 3D SYSTEMS. All rights reserved.



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HapticGUI;
using RosSharp.RosBridgeClient;
using System.Threading;


//using StackableDecorator;
#if UNITY_EDITOR
using UnityEditor;
#endif

class HapticCollider : MonoBehaviour
{
    public HapticPlugin HPlugin = null;
    //public GrabberControl GControl = null;

    private int NumHitsDampingMax = 3;
    private int NumHitsDamping = 0;

    [Label(title = "Stiffness")]
    [Slider(0, 1)]
    public float hStiffness = 1.0f;

    [Header("Friction Properties")]
    [Label(title = "Static Friction")]
    [Slider(0, 1)]
    public float hFrictionS = 0.0f;
    [Label(title = "Dynamic Friction")]
    [Slider(0, 1)]
    public float hFrictionD = 0.0f;
    private MeshRenderer Renderer_mesh;
    public Material DefaultMaterial;
    private Collider Collider_mesh;
    public Material CollisionMaterial;

    public Initialization_Voxel voxel_world;
    public Chunk2 chunk2;
    public Transform Hapticposition;

    public string deviceName;
    public int switched;
    public int countnum;
    public double[] usable6;
    public double[] max6;

    public Vector3 contactpoint;

    void Start()
    {
        //world = GameObject.Find("Voxel").GetComponent<World>();
        //chunk2 = GameObject.Find("Voxel2").GetComponent<Chunk2>();
        //voxel_world = GameObject.Find("Voxel Initializer").GetComponent<Initialization_Voxel>();

        if (HPlugin == null)
        {

            HapticPlugin[] HPs = (HapticPlugin[])UnityEngine.Object.FindObjectsOfType(typeof(HapticPlugin));
            foreach (HapticPlugin HP in HPs)
            {
                if (HP.CollisionMesh == this.gameObject)
                {
                    HPlugin = HP;
                }
            }

        }
        Renderer_mesh = transform.Find("HapticEnd").GetComponent<MeshRenderer>();
        Collider_mesh = transform.Find("HapticEnd").GetComponent<CapsuleCollider>();
        
        //HapticPlugin.getWorkspaceArea(deviceName, usable6, max6);

    }

    void Update()
    {
        //double magnitude = 1 / 79.0;
        //double[] direction3 = new double[] { 0, 0, 1 };
        //HapticPlugin.setConstantForceValues(deviceName, direction3, magnitude);

        //double[] lateral3 = new double[] { 0, 0, 1 };
        //double[] torque3 = new double[] { 0, 0, 1, 0, 0, 0 };
        //HapticPlugin.setForce(deviceName, lateral3, torque3);

        double[] seeforce = new double[] { 0, 0, 0 };
        HapticPlugin.getCurrentForce(deviceName, seeforce);
        //print($"{seeforce[0]}, {seeforce[1]}, {seeforce[2]}");
        double operatorforce = Math.Sqrt(seeforce[0] * seeforce[0]+ seeforce[1] * seeforce[1] + seeforce[2] * seeforce[2]);


        if ( operatorforce > 0.1 )
        {
            //print(contactpoint);
            chunk2.EditVoxel(contactpoint, 0);
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<HapticCollider>() == null)
        {
            if (HPlugin != null)
            {
                //Debug.Log("update collision");
                HPlugin.UpdateCollision(collision, true, false, false);
                // GControl.GrabberCollision(collision, gameObject);
                HPlugin.enable_damping = true;
            }
        }
        print($"Contacted {collision.gameObject.name}");

        
        //world.GetChunkFromVector3(contactpoint).EditVoxel(contactpoint, false);
        //voxel_world.EditVoxel(contactpoint, 0);
        //chunk2.EditVoxel(contactpoint, 0);


    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.GetComponent<HapticCollider>() == null)
        {
            if (HPlugin != null)
            {
                if (NumHitsDamping >= NumHitsDampingMax)
                {
                    HPlugin.enable_damping = false;
                }
                else
                {
                    NumHitsDamping++;
                }

               HPlugin.UpdateCollision(collision, false, true, false);

            }
        }
        //countnum = 0;
        foreach (ContactPoint contact in collision.contacts)
        {
            //countnum++;
            //print(countnum);
            contactpoint = collision.contacts[0].point;
            print(collision.contactCount);
            //print(collision.contacts);
            if (contact.thisCollider == Collider_mesh)
            {
                Renderer_mesh.material = CollisionMaterial;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.GetComponent<HapticCollider>() == null)
        {
            if (HPlugin != null)
            {
                HPlugin.UpdateCollision(collision, false, false, true);
                NumHitsDamping = 0;
            }
        }
        Renderer_mesh.material = DefaultMaterial;
        //chunk2.EditVoxel(contactpoint, 0);
    }




}

