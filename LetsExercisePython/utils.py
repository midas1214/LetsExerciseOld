import cv2
import numpy as np

def find_angle(p1, p2, ref_pt):
    p1_ref = p1 - ref_pt
    p2_ref = p2 - ref_pt

    cos_theta = (np.dot(p1_ref,p2_ref)) / (1.0 * np.linalg.norm(p1_ref) * np.linalg.norm(p2_ref))
    theta = np.arccos(np.clip(cos_theta, -1.0, 1.0))
            
    degree = int(180 / np.pi) * theta

    return int(degree)

def get_landmark_array(pose_landmark, key, frame_width, frame_height):
    denorm_x = int(pose_landmark[key][0]*frame_width)
    denorm_y = int(pose_landmark[key][1]*frame_height)

    return np.array([denorm_x, denorm_y])




def get_landmark_features(kp_results, dict_features, feature, frame_width, frame_height):
    point1 = get_landmark_array(kp_results,dict_features[feature["point1"]],frame_width,frame_height)
    point2 = get_landmark_array(kp_results,dict_features[feature["point2"]],frame_width,frame_height)
    ref_point = get_landmark_array(kp_results,dict_features[feature["ref_point"]],frame_width,frame_height)

    return point1,point2,ref_point
    
    