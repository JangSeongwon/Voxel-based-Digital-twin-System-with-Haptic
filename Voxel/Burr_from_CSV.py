# Python 3.11.9 버전 작성

import pandas as pd
from decimal import Decimal
import numpy as np

file_path = "D:\\Voxel-based digital twin 자료\\3D Scanner\\250217_testing_data\\2330_test\\2330_sample.csv"

'''
CSV의 1행 = pandas 0행 
'''

raw_data = pd.read_csv(file_path)
# print(raw_data)
# print(raw_data.iloc[26,0].dtype)

"전체 Datasheet 범위"
# point_cloud_data = raw_data.iloc[27:1058,1:742]
# print(point_cloud_data)

"1번 Burr 영역 범위"
'Unity에는 해당 좌표계로 형성됨'
'Voxel size = 0.1mm / 기준 Chunk 1개: 5mm X 5mm (50개)'
raw_point_cloud_data = raw_data.iloc[550:750, 400:500]
point_cloud_data = raw_point_cloud_data.fillna(0)
# print(point_cloud_data)

point_cloud_data = point_cloud_data.astype(float)
# print(point_cloud_data.iloc[26,0], point_cloud_data.iloc[26,0].dtype)

'사전 정보로 Surface 나누기'
Surface_group_1 = (point_cloud_data <= -6.5) & (point_cloud_data >= -7.5)
Surface_group_2 = (point_cloud_data <= -3.5) & (point_cloud_data >= -4.5)
Surface_group_1_values = point_cloud_data[Surface_group_1]
Surface_group_2_values = point_cloud_data[Surface_group_2]
# print(Surface_group_1_values)

'각 Surface의 높이 구하기'
surface_roughness_average_1 = Surface_group_1_values.mean().mean()
surface_roughness_average_2 = Surface_group_2_values.mean().mean()
print("Surface Roughness Area 1: ", surface_roughness_average_1, "Surface Roughness Area 2: ", surface_roughness_average_2)

Surface_group_1_values.to_csv( 'D:\\Voxel-based digital twin 자료\\3D Scanner\\Surface확인CSV\\surface_1.csv', index=False)
Surface_group_2_values.to_csv( 'D:\\Voxel-based digital twin 자료\\3D Scanner\\Surface확인CSV\\surface_2.csv', index=False)

Surface_2_Burr_extraction = Surface_group_2_values - surface_roughness_average_2
Surface_2_Burr_extraction[Surface_2_Burr_extraction < 0.15] = np.nan

Surface_2_Burr_extraction.to_csv( 'D:\\Voxel-based digital twin 자료\\3D Scanner\\Surface확인CSV\\Burr_surface_2.csv', index=False)

''' Burr 있는 부분만 추출해서 Voxel화 하려고 만든 구간간 '''
Seeing_Surface_2_Burr_extraction = Surface_2_Burr_extraction.iloc[81:137, 46:87]
# Surface_2_Burr_extraction.to_csv( 'D:\\Voxel-based digital twin 자료\\3D Scanner\\Surface확인CSV\\Burr_only_surface_2.csv', index=False)
Seeing_Surface_2_Burr_extraction = Seeing_Surface_2_Burr_extraction.applymap(lambda x: int(x / 0.01) if pd.notna(x) else np.nan)
Seeing_Surface_2_Burr_extraction = Seeing_Surface_2_Burr_extraction.values
Seeing_Surface_2_Burr_extraction = pd.DataFrame(Seeing_Surface_2_Burr_extraction)
Seeing_Surface_2_Burr_extraction.to_csv( 'D:\\Voxel-based digital twin 자료\\3D Scanner\\Surface확인CSV\\Seeing_Burr_voxelnum_surface_2_Chunk1.csv', index=False)
print("Burr 높이 보기", Seeing_Surface_2_Burr_extraction.values)


''' 이론 상 전체 .CSV 에서 Chunk 나누는 구간'''
# Surface_2_Burr_extraction = Surface_2_Burr_extraction.applymap(lambda x: int(x / 0.01) if pd.notna(x) else np.nan)
# Surface_2_Burr_extraction.to_csv( 'D:\\Voxel-based digital twin 자료\\3D Scanner\\Surface확인CSV\\Burr_voxelnum_surface_2.csv', index=False)



