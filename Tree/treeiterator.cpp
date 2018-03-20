#include "treeiterator.h"


template<class T>
TreeIterator<T>::TreeIterator()
{
    originalTree = 0;
    stack = new QStack<TreeNode<T>*>();
}
template<class T>
TreeIterator<T>::TreeIterator(TreeRB<T>* tree)
{
    originalTree = tree;
    stack = new QStack<TreeNode<T>*>();
}

template<class T>
void TreeIterator<T>::Inc()
{
    TreeNode<T>* currentNode;
    if (stack->isEmpty())
    {
        currentNode = originalTree->getRoot();
        currentNode->checked = true;
        stack->push(currentNode);
        return;
    }
    else
        currentNode = stack->first();

    // was here
    currentNode->checked = true;
    cout << "Visit a " << currentNode->getDate() << endl;

    while ((!currentNode->left && !currentNode->right) && currentNode != originalTree->getRoot() )
        stack->pop();

    if (currentNode->left && !currentNode->left->checked)
    {
        currentNode = currentNode->left;
        stack->push(currentNode);
        return;
    }
    if (currentNode->right && !currentNode->right->checked)
    {
        currentNode = currentNode->right;
        stack->push(currentNode);
        return;
    }
    // no place to go
    cout << "The tree finished!" << endl;
    return;
}

template<class T>
TreeNode<T> *TreeIterator<T>::get()
{
    if (stack->isEmpty())
        return 0;
    return stack->first();
}
