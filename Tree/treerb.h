#ifndef TREERB_H
#define TREERB_H

#include "treenode.h"

template <class T>
class TreeRB
{
    TreeNode<T>* root;
public:
    TreeRB<T>(){ root = NULL; }
    TreeRB<T>(T arr[]);
    ~TreeRB<T>(){ delete root; }
    void Trace (int depth);



    TreeNode<T> *grandParent (TreeNode<T> *of);
    TreeNode<T> *uncle (TreeNode<T> *of);

    void rotateLeft (TreeNode<T> * n);
    void rotateRight (TreeNode<T> * n);

    void insertCase1 (TreeNode<T> *n);
    void insertCase2 (TreeNode<T> *n);
    void insertCase3 (TreeNode<T> *n);
    void insertCase4 (TreeNode<T> *n);
    void insertCase5 (TreeNode<T> *n);

    void deleteOneChild (TreeNode<T> *n);

    void deleteCase1 (TreeNode<T> *n);
    void deleteCase2 (TreeNode<T> *n);
    void deleteCase3 (TreeNode<T> *n);
    void deleteCase4 (TreeNode<T> *n);
    void deleteCase5 (TreeNode<T> *n);
    void deleteCase6 (TreeNode<T> *n);
};

#endif // TREERB_H
