import cv2
from cvzone.PoseModule import PoseDetector
import socket
import json

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
cap.set(3, width)
cap.set(4, height)

# Pose Detector
detector = PoseDetector()

#posList = []
# Communication
udp_sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)  # UDP
serverAddressPort = (udp_ip, udp_port)

while True:
    success, img = cap.read()
    img = detector.findPose(img)
    img = cv2.flip(img, 1)
    lmList, bboxInfo = detector.findPosition(img)

    #tcp
    img_data = {'image':cv2.imencode('.jpg',img)[1].ravel().tolist()}
    data = json.dumps(img_data)

    #準備連線
    tcp_sock = socket.socket (socket.AF_INET, socket.SOCK_STREAM) # TCP
    tcp_sock.connect(address)
    #傳送資料
    tcp_sock. sendall(bytes (data,encoding='utf-8'))

    #udp
    data = []
    if bboxInfo:
        # Get the landmark list
        #lmString = ''
        for landmark in lmList:
            data.extend([landmark[1], height - landmark[2], landmark[3]])
            #lmString += f'{landmark[1]}, {img.shape[0] - landmark[2]}, {landmark[3]}'

        #posList.append(lmString)

        udp_sock.sendto(str.encode(str(data)), serverAddressPort)

    cv2.imshow("Image", img)
    cv2.waitKey(10)
    tcp_sock.close()

    if cv2.waitKey(1) == ord('q'):
        break  # press q to quit

