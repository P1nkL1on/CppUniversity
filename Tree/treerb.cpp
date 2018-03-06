#include "treerb.h"


template<class T>
TreeRB<T>::TreeRB(T arr[])
{

}

template<class T>
void TreeRB<T>::Trace(int depth)
{
    root->TraceInfo(depth);
}

template<class T>
TreeNode<T> *TreeRB<T>::grandParent(TreeNode<T> *of)
{
    if (of != NULL && of->parent != NULL)
        return of->parent->parent;
    return NULL;
}

template<class T>
TreeNode<T> *TreeRB<T>::uncle(TreeNode<T> *of)
{
    if (of == NULL)
        return NULL;
    TreeNode<T> *gran = grandParent(of);
    if (gran == NULL)
        return NULL;
    if (of->parent == gran->left)
        return gran->right;
    return gran->left;
}

template<class T>
void TreeRB<T>::rotateLeft(TreeNode<T> *n)
{
       TreeNode<T> *pivot = n->right;
       pivot->parent = n->parent;

       if (n->parent != NULL)
       {
           if (n->parent->left==n)
               n->parent->left = pivot;
           else
               n->parent->right = pivot;
       }

       n->right = pivot->left;
       if (pivot->left != NULL)
           pivot->left->parent = n;

       n->parent = pivot;
       pivot->left = n;
}

template<class T>
void TreeRB<T>::rotateRight(TreeNode<T> *n)
{
    struct node *pivot = n->left;

    pivot->parent = n->parent;
    if (n->parent != NULL) {
        if (n->parent->left==n)
            n->parent->left = pivot;
        else
            n->parent->right = pivot;
    }

    n->left = pivot->right;
    if (pivot->right != NULL)
        pivot->right->parent = n;

    n->parent = pivot;
    pivot->right = n;
}

template<class T>
void TreeRB<T>::insertCase1(TreeNode<T> *n)
{
    if (n->parent == NULL)
        n->color = 'b';
    else
        insertCase2(n);
}
template<class T>
void TreeRB<T>::insertCase2(TreeNode<T> *n)
{
    if (n->parent->color == 'b')
        return;
    insertCase3(n);
}

template<class T>
void TreeRB<T>::insertCase3(TreeNode<T> *n)
{
    TreeNode<T> *u = uncle(n), *g;
    if (u != NULL && u->color == 'r'){
        n->parent->color = 'b';
        u->color = 'b';
        g = grandParent(n);
        g->color = 'r';
        insertCase1(g);
    }else
        insertCase4(n);
}

template<class T>
void TreeRB<T>::insertCase4(TreeNode<T> *n)
{
    TreeNode<T>* g = grandParent(g);
    if (n == n->parent->right && n->parent == g->left)
    {
        rotateLeft(n->parent);
        n = n->left;
    } else
        if (n == n->parent->left && n->parent == g->right){
            rotateRight(n->parent);
            n = n->right;
        }
    insertCase5(n);
}

template<class T>
void TreeRB<T>::insertCase5(TreeNode<T> *n)
{
    TreeNode<T> *g = grandParent(n);
    n->parent->color = 'b';
    g->color = 'r';
    if (n == n->parent->left && n->parent == g->left)
        rotateRight(g);
    else
        rotateLeft(g);
}
