# I2CPS Digital-Twin 
Haptic - Unity - ROS 

--------------------------
*Unity Project Features*

1. Haptic Touch X in Unity    
Use Assets: https://assetstore.unity.com/packages/tools/integration/haptics-direct-for-unity-v1-197034
```
1. Actor : [HapticPlugin]
2. Collider : [HapticCollider]
3. Haptic Initializer : [Initialization_Haptic]
4. Voxel Initializer : [Chunk2]
```

2. Voxelization in Unity    

```   
Voxel size = 0.1mm

Chunk Path: Unity 자료\\I2CPS-M1509\\Assets\\Chunk path

```   

3. Doosan Robot in Unity
```
M1509 (6DOF): URDF
*Currently with 6FTsensor and Simple Motor-Drill

m1509_ori: Need to change Links(Rigid Body) into Articulation Body (M1509 Reference to copy and paste its components)

Collision : Adding Collisions
Moved with Joint inputs: [SetJointPositions in Initialization]

Coordinate Transfrom
-Formed calculation function for Haptic Orientation and ROS Orientation
ROS topic : /HapticOri [EndEffectorPosePublisher]

+Other used Assets
 - Grid Material 'https://assetstore.unity.com/packages/2d/textures-materials/gridbox-prototype-materials-129127?srsltid=AfmBOoowjbEvLqTxcaygvjJtN8hxCxJowGYixQxBeUa_0jl3p8vigAHM'

```

4. 6F/T Sensor
```
ROS topic: /robotiq_ft_sensor
Subscriber: ForceFeedbackSubscriber
Feedback to Haptic: [HapticPlugin.setForce(deviceName, lateral3, torque3);]
```


--------------------------
*ROS with Unity*

ROS#: https://github.com/siemens/ros-sharp   
*URDF Importer: https://github.com/Unity-Technologies/URDF-Importer (ROSBridge URDF Importer와 충돌, ROS#만 사용)   

1. Haptic Touch X
```
From Haptic
Haptic information(Transform) sending: Use [EndEffectorPublisher.cs] from RosSharp
ROS topic Published (Pose): '/HapticInfo' in 50Hz
ROS topic Published (Differential Orientation): '/HapticOri' in 50Hz

```

2. ROS
```
Using Rviz(Doosan Robot ROS Package)
1. I2CPS_haptic (alias in gedit ~/.bashrc) : launching virtual mode of doosan robot simulation (jsw_haptic.launch)
2. Python3 (Haptic_Main.py) : Moving Robot eef with [movel] & Haptic POS

ROS topic to Unity: [JointStatePublisher] '/dsr01m1509/joint_states'
ROS topic subscribed in Unity with: [JointValueSubscriber] (msg type: Sensor.JointState)

*Moveit IK Solver timing
Current vel 500, acc 2000 / Threshold 1mm, 0.1degrees / Sensitivity S = 500   

Workspace
Distinguisher as ROS topic: /ToolEnd
Global Coordinate limitation of the workspace: (x, y, z) = (-0.15 ~ 0.15, -0.005 ~ 0.16, 0.25 ~ 0.55) 30cm Cube with Target at center

Coordinate Settings
u1 = [0,-1,0]
v1 = [0,0,1]
O = (0,0,0)

Data Communication Speed settings:
https://answers.ros.org/question/332192/difference-between-rospyspin-and-rospysleep/

Python코드 : chmod +x [file명]

```


--------------------------

