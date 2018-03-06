#ifndef TREENODE_H
#define TREENODE_H

#include "cstdlib"
#include "iostream"
#include <stdio.h>
using namespace std;
#include "QDebug"

template <class T>
class TreeNode
{
    T date;
    TreeNode *left;
    TreeNode *right;
    char color;
public:
    TreeNode(){
        cout << "+T";
        date = T();
        color = 'b';
        left = right = NULL;
    }
    TreeNode(bool isBlack, T d){
        date = d;
        color = (isBlack)? 'b' : 'r';
        left = right = NULL;
        cout << "+T (" << color <<")";
    }

    int SetChild (TreeNode *child){
        cout << left << "_" << right << endl;
        if (left == 0x0)
        {
            left = child;
            return 0;
        }
        if (right == 0x0)
        {
            right = child;
            return 1;
        }
        return -1;
    }
    T getDate()
    {
        return date;
    }
    int TraceInfo (int depth){
        int traceCnt = 0, dep = depth;
        while (dep--)
            cout << " ";
        cout << color << " " << getDate() << endl;

        if (left)
            traceCnt += left->TraceInfo(depth + 1);
        if (right)
            traceCnt += right->TraceInfo(depth + 1);
        return traceCnt;
    }
};

#endif // TREENODE_H
