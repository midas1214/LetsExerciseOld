import cv2
from cvzone.PoseModule import PoseDetector
from cvzone.HandTrackingModule import HandDetector
import socket
from utils import find_angle, get_landmark_features
from pose_dataset import get_pose_db
import json

# 取得資料庫
pose_db = get_pose_db()

check_point = pose_db["pose_name"]["check_angle"]
file_path = pose_db["pose_name"]["path"]
video_frame_width = pose_db["pose_name"]["video_size"]["width"]
video_frame_height = pose_db["pose_name"]["video_size"]["height"]


with open(file_path, "r") as lm_file:
    lines = lm_file.readlines()

#tcp
tcp_ip = '127.0.0.1'
tcp_port = 5066
address = (tcp_ip,tcp_port)

#udp
udp_ip = '127.0.0.1'
udp_port = 5052

# Parameters
width, height = 1280, 1000

# Webcam
cap = cv2.VideoCapture(0)
if not cap.isOpened():
    print("Cannot open camera")
    exit()

# Get the actual width and height of the webcam frames
frame_width = int(cap.get(3))
frame_height = int(cap.get(4))

# Pose Detector
pose_detector = PoseDetector()
hand_detector = HandDetector(detectionCon=0.8, maxHands=2)

# Communication
udp_sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)  # UDP
serverAddressPort = (udp_ip, udp_port)

dict_features = {
    'right_shoulder': 12,
    'right_elbow'   : 14,
    'right_wrist'   : 16,
    'right_hip'     : 24,
    'right_knee'    : 26,
    'right_ankle'   : 28,
    'right_foot'    : 32,
    'left_shoulder': 11,
    'left_elbow'   : 13,
    'left_wrist'   : 15,                    
    'left_hip'     : 23,
    'left_knee'    : 25,
    'left_ankle'   : 27,
    'left_foot'    : 31,
    'nose' : 0
}
wrong_message = "nice"
counter = 0 #video_counter

while counter < len(lines):
    success, img = cap.read()
    img = cv2.flip(img, 1)
    # 抓身體的點
    img = pose_detector.findPose(img)
    lmList, bboxInfo = pose_detector.findPosition(img)

    #tcp
    img_data = {'image':cv2.imencode('.jpg',img)[1].ravel().tolist()}
    data = json.dumps(img_data)
    #準備連線
    tcp_sock = socket.socket (socket.AF_INET, socket.SOCK_STREAM) # TCP
    tcp_sock.connect(address)
    #傳送資料
    tcp_sock.sendall(bytes (data,encoding='utf-8'))

    # 抓手的點
    hands, imgg = hand_detector.findHands(img)

    #cv2.imshow("Image",imgg)

    if hands:
        hand = hands[0] 
        hand_lmlist = hand['lmList']
        fingers = hand_detector.fingersUp(hand)
        if fingers == [0,1,0,0,0]:
            # 傳送食指 x , y 值
            index_finger = hand_lmlist[8][0], hand_lmlist[8][1]
            index_finger_json = json.dumps(index_finger)
            udp_sock.sendto(str.encode(index_finger_json), serverAddressPort)
        # 兩隻手指頭 點擊
        if fingers == [0,1,1,0,0]:
            index_finger = hand_lmlist[8][0], hand_lmlist[8][1],1
            index_finger_json = json.dumps(index_finger)
            udp_sock.sendto(str.encode(index_finger_json), serverAddressPort)




    points = lines[counter].split(',')
    video_lmlist = [[int(point) for point in points[i:i+3]] for i in range(0, len(points)-1, 3)]
    wrong_message = "nice"

    #for index in range(len(check_point)):
        #point1,point2,ref_point = get_landmark_features(lmList,dict_features,check_point[index],frame_width,frame_height)
        #video_point1,video_point2,video_ref_point = get_landmark_features(video_lmlist,dict_features,check_point[index],video_frame_width,video_frame_height)
        #offset_angle = find_angle(point1,point2,ref_point)
        #video_offset_angle = find_angle(video_point1,video_point2,video_ref_point)
        #wrong = offset_angle - video_offset_angle
        #wrong = abs(wrong)
        #這裡comparison先隨便寫的
        #if wrong > 10 :
        #    wrong_message = "too high"

    counter+=1

    #udp
    #udp_sock.sendto(str.encode(wrong_message), serverAddressPort)


    #cv2.imshow("Image", img)
    tcp_sock.close()

    if cv2.waitKey(30) == ord('q'):
        break  # press q to quit

