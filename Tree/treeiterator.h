#ifndef TREEITERATOR_H
#define TREEITERATOR_H

#include "treerb.h"
#include "qstack.h"

template <class T>
class TreeIterator
{
    TreeRB<T>* originalTree;
    QStack<TreeNode<T>*>* stack;

public:
    TreeIterator();
    TreeIterator(TreeRB<T>* tree);
    void Inc();
    TreeNode<T>* get ();
};

#endif // TREEITERATOR_H
