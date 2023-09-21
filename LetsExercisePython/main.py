import cv2
from cvzone.PoseModule import PoseDetector
import socket

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
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)  # UDP
serverAddressPort = ("127.0.0.1", 5052)

while True:
    success, img = cap.read()
    img = detector.findPose(img)
    img = cv2.flip(img, 1)
    lmList, bboxInfo = detector.findPosition(img)

    data = []
    if bboxInfo:
        # Get the landmark list
        #lmString = ''
        for landmark in lmList:
            data.extend([landmark[1], height - landmark[2], landmark[3]])
            #lmString += f'{landmark[1]}, {img.shape[0] - landmark[2]}, {landmark[3]}'

        #posList.append(lmString)

        sock.sendto(str.encode(str(data)), serverAddressPort)

    cv2.imshow("Image", img)
    if cv2.waitKey(1) == ord('q'):
        break  # press q to quit

