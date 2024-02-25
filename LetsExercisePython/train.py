import torch
import torchvision.models as models
from torchsummary import summary
import torch.nn as nn
import torch.optim as optim
import torchvision
from torchvision import datasets
import torch.nn.functional as F
import torchvision.transforms as transforms
import matplotlib.pyplot as plt
import numpy as np

if __name__ == '__main__':

    model = torchvision.models.resnet50(pretrained=True)
    num_ftrs = model.fc.in_features
    model.fc = nn.Linear(num_ftrs, 2)

    #model = model.cuda()

    train_transform = transforms.Compose([
        transforms.Resize((224, 224)),
        transforms.RandomHorizontalFlip(),
        transforms.RandomVerticalFlip(),
        transforms.RandomRotation(30),
        transforms.ToTensor(),
        transforms.RandomErasing(),
        transforms.Normalize((0.5,), (0.5,))
    ])

    valid_transform = transforms.Compose([
        transforms.Resize((224, 224)),
        transforms.ToTensor(),
        transforms.Normalize((0.5,), (0.5,))
    ])

    train_dataset = datasets.ImageFolder(root='./dataset/train', transform=train_transform)
    valid_dataset = datasets.ImageFolder(root='./dataset/validate', transform=valid_transform)

    train_loader = torch.utils.data.DataLoader(train_dataset, batch_size=16, shuffle=True, num_workers=2)
    valid_loader = torch.utils.data.DataLoader(valid_dataset, batch_size=16, shuffle=False, num_workers=2)

    # Define the loss function and optimizer
    criterion = nn.CrossEntropyLoss()
    optimizer = torch.optim.Adam(model.parameters(), lr=1e-4)

    num_epoch = 30
    train_acc = []
    train_loss = []
    val_acc = []
    val_loss = []
    best_val_acc = 0
    best_epoch = 0


    for epoch in range(num_epoch):
        model.train()
        train_loss_history = []
        train_acc_history = []
        for x,y in train_loader :
            #x , y = x.cuda() , y.cuda()
            y_one_hot = nn.functional.one_hot(y,num_classes=2).float()
            y_pred = model(x)
            loss = criterion(y_pred,y_one_hot)
            loss.backward()
            optimizer.step()
            optimizer.zero_grad()
            acc = (y_pred.argmax(dim = 1) == y).float().mean()
            train_loss_history.append(loss.item())
            train_acc_history.append(acc.item())
        train_loss.append(sum(train_loss_history)/len(train_loss_history))
        train_acc.append(sum(train_acc_history)/len(train_acc_history))

        model.eval()
        val_loss_history = []
        val_acc_history = []
        for x,y in valid_loader :
            #x , y = x.cuda() , y.cuda()
            y_one_hot = nn.functional.one_hot(y,num_classes=2).float()
            with torch.no_grad():
                y_pred = model(x)
                loss = criterion(y_pred,y_one_hot)
                acc = (y_pred.argmax(dim = 1) == y).float().mean()
            val_loss_history.append(loss.item())
            val_acc_history.append(acc.item())
        val_loss.append(sum(val_loss_history)/len(val_loss_history))
        val_acc.append(sum(val_acc_history)/len(val_acc_history))

        avg_val_acc = sum(val_acc_history)/len(val_acc_history)

        if avg_val_acc >= best_val_acc:
            print("Best model saved at epoch {} ,acc: {:.4f}".format(epoch,avg_val_acc))
            best_val_acc = avg_val_acc
            best_epoch = epoch
            torch.save(model.state_dict(), "best_model5_erase.pth")










