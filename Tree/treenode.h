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

public:
    TreeNode<T> *left;
    TreeNode<T> *right;
    TreeNode<T> *parent;
    char color;

    TreeNode<T>(){
        //cout << "+T";
        date = T();
        color = 'r';
        left = right = parent = NULL;
    }
    TreeNode<T>(T d){
        date = d;
        color = 'r';
        left = right = parent = NULL;
        //cout << "+T (" << color <<")";
    }
    ~TreeNode<T>(){
        if (!left)
            delete left;
        if (!right)
            delete right;
        if (!parent)
            delete parent;
        //
    }

    int SetChild (TreeNode<T> *child){
        //cout << left << "_" << right << endl;
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
