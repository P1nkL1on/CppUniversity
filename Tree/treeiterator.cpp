#include "treeiterator.h"


template<class T>
TreeIterator<T>::TreeIterator()
{
    originalTree = 0;
    stack = new QStack<TreeNode<T>*>();
    finished = true;
}
template<class T>
TreeIterator<T>::TreeIterator(TreeRB<T>* tree)
{
    originalTree = tree;
    stack = new QStack<TreeNode<T>*>();
    finished = false;
}

template<class T>
bool TreeIterator<T>::isEnd()
{
   return finished;
}

template<class T>
TreeIterator<T>& TreeIterator<T>::operator++()
{
    //cout << "++" << endl;
    TreeNode<T>* currentNode;
    if (stack->isEmpty())
    {
        currentNode = originalTree->getRoot();
        currentNode->checked = true;
        stack->push(currentNode);
        return *this;
    }
    else{
        currentNode = stack->pop();
        stack->push(currentNode);
    }

    // was here
    currentNode->checked = true;
    //cout << "Visit a " << currentNode->getDate() << endl;

    while ((!currentNode->left && !currentNode->right)
           ||
           (currentNode->checked && (!currentNode->left || (currentNode->left && currentNode->left->checked))
                                 && (!currentNode->right || (currentNode->right && currentNode->right->checked)))){
        if (!(stack->isEmpty()))
            currentNode = stack->pop();
        else{
            finished = true;
            return *this;
        }
    }

    if (currentNode->left && !currentNode->left->checked)
    {
        currentNode = currentNode->left;
        stack->push(currentNode);
        return *this;
    }
    if (currentNode->right && !currentNode->right->checked)
    {
        currentNode = currentNode->right;
        stack->push(currentNode);
        return *this;
    }
    // no place to go
    //cout << "The tree finished!" << endl;
    return *this;
}

template<class T>
TreeNode<T> *TreeIterator<T>::operator *()
{
    if (stack->isEmpty())
        return 0;
    return stack->last();
}
