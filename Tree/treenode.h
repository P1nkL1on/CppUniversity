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
    TreeNode *parent;
    char color;
public:
    TreeNode<T>(){
        cout << "+T";
        date = T();
        color = 'b';
        left = right = NULL;
    }
    TreeNode<T>(bool isBlack, T d){
        date = d;
        color = (isBlack)? 'b' : 'r';
        left = right = NULL;
        cout << "+T (" << color <<")";
    }
    ~TreeNode<T>(){
        if (left)
            delete left;
        if (right)
            delete right;
        delete parent;
        //
    }

    int SetChild (TreeNode<T> *child){
        cout << left << "_" << right << endl;
        child->parent = this;
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

        child->parent = NULL;
        return -1;
    }
    T getDate()
    {
        return date;
    }
    int TraceInfo (int depth){
        int traceCnt = 1, dep = depth;
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
