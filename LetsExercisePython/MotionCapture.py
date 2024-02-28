import cv2
from cvzone.PoseModule import PoseDetector

cap = cv2.VideoCapture('video.mp4')
detector = PoseDetector()

postList = []

while True:
    success , img = cap.read()
    img = detector.findPose(img)
    lmList,bboxInfo = detector.findPosition(img)

    #if the bounding box is not empty
    if bboxInfo:
       lmString = ''
       for lm in lmList:
          lmString += f'{lm[0]},{img.shape[0] - lm[1]},{lm[2]},'
       with open("lmList.txt",'a') as f:
          f.write(lmString)
          f.write("\r\n")
       
    cv2.imshow("Image",img)

    if cv2.waitKey(1) == ord('q'):
         break
       
