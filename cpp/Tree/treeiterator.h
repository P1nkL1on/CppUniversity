#ifndef TREEITERATOR_H
#define TREEITERATOR_H

#include "treerb.h"
#include "qstack.h"

template <class T>
class TreeIterator
{
    TreeRB<T>* originalTree;
    QStack<TreeNode<T>*>* stack;
    bool finished;
public:
    TreeIterator();
    TreeIterator(TreeRB<T>* tree);
    bool isEnd();
    TreeIterator<T>& operator++();
    TreeNode<T>* operator * ();
    //TreeNode<T>* get ();
};

#endif // TREEITERATOR_H
