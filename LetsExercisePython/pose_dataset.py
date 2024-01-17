
pose = {
    "pose_name":
    {
        "path" : "video_lmlist/lmList.txt",
        "check_angle":[
            {
                "point1": "left_elbow",
                "point2": "right_shoulder",
                "ref_point":"left_shoulder"
            },
            {
                "point1": "right_elbow",
                "point2": "left_shoulder",
                "ref_point": "right_shoulder",
            }
        ],
        "video_size":{
            "width" :1920,
            "height":1080
        }
    }
    
}

def get_pose_db():
    return pose