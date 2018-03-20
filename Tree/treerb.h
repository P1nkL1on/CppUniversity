#ifndef TREERB_H
#define TREERB_H

#include "callback.h"

template <class T>
class TreeRB
{
protected:

    TreeNode<T>* root;

    TreeNode<T> *grandParent (TreeNode<T> *of);
    TreeNode<T> *uncle (TreeNode<T> *of);

    void rotateLeft (TreeNode<T> * n);
    void rotateRight (TreeNode<T> * n);

    void refindRoot (TreeNode<T> *from);

    void insertCase1 (TreeNode<T> *n);
    void insertCase2 (TreeNode<T> *n);
    void insertCase3 (TreeNode<T> *n);
    void insertCase4 (TreeNode<T> *n);
    void insertCase5 (TreeNode<T> *n);

    TreeNode<T>* subling (TreeNode<T>*n){
        if (n == n->parent->left)
            return n->parent->right;
        return n->parent->left;
    }


    void deleteCase1 (TreeNode<T> *n);
    void deleteCase2 (TreeNode<T> *n);
    void deleteCase3 (TreeNode<T> *n);
    void deleteCase4 (TreeNode<T> *n);
    void deleteCase5 (TreeNode<T> *n);
    void deleteCase6 (TreeNode<T> *n);
    TreeNode<T>* findRecursive (TreeNode<T>* nowNode, T value);
    int DeepByPassRecursive (TreeNode<T>* nowNode, CallBack calback);

public:
    TreeNode<T>* getRoot();
    void deleteOneChild (TreeNode<T> *n);
    TreeRB(){ root = NULL; }
    TreeRB(T arr[], int count);
    ~TreeRB<T>(){
        delete root;
    }
    void DeepByPass (CallBack callback);

    void AddComponent (T value);

    TreeNode<T>* Find (T value);

    TreeNode<T>* FindBiggest (TreeNode<T>* currentNode){
        if (currentNode->left == NULL && currentNode->right == NULL)
            return currentNode;
        if ((currentNode->left!=NULL) * 1 + (currentNode->right!=NULL) * 1 == 1)
        {
            if (currentNode->left) return FindBiggest(currentNode->left);
            if (currentNode->right) return FindBiggest(currentNode->right);
        }
        T left = FindBiggest(currentNode->left)->getDate(),
          right = FindBiggest(currentNode->right)->getDate();
        return (left < right)? currentNode->right : currentNode->left;
    }
};

#endif // TREERB_H
