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

    //static int NodeCountInMemory;
public:
    bool checked;
    TreeNode<T> *left;
    TreeNode<T> *right;
    TreeNode<T> *parent;
    char color;

    TreeNode<T>(){
        //NodeCountInMemory++;
        date = T();
        color = 'r';
        left = right = parent = NULL;
        checked = false;
    }
    TreeNode<T>(T d){
        //TreeNode<T>::NodeCountInMemory++;
        date = d;
        color = 'r';
        left = right = parent = NULL;
        //cout << "+T (" << color <<")";
        checked = false;
    }
    ~TreeNode<T>(){
        if (parent){
            if (parent->left == this)
                parent->left = NULL;
            else
                parent->right = NULL;
        }
        if (left)
            delete left;
        if (right)
            delete right;
//        left = right = NULL;
        //TreeNode<T>::NodeCountInMemory--;
    }

    int SetChild (TreeNode<T> *child){
        //cout << "--------   " << child->date << "  is now child of " << this->date << endl;
        child->parent = this;
        if (!left && !right){
            //cout << "check with all kids 0x0" << endl;
            if (this->date < child->date)
                right = child;
            else
                left = child;
            return 0;
        }
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
    void setDate (TreeNode<T>*of){
        date = of->date;
    }

    T getDate()
    {
        return date;
    }

};

#endif // TREENODE_H
