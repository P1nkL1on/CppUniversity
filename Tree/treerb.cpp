#include "treerb.h"


template<class T>
TreeRB<T>::TreeRB(T arr[], int count)
{
    for (int i = 0; i < count; i++){
        TreeNode<T>* newNode = new TreeNode<T>(arr[i]);
        insertCase1(newNode);
    }
}
template<class T>
void TreeRB<T>::Trace(int depth)
{
    root->TraceInfo(depth);
}

template<class T>
TreeNode<T> *FindAChildlessNode (TreeNode<T>*currentNode){
    if (!currentNode->left || !currentNode->right)
        return currentNode;
    TreeNode<T>* post = FindAChildlessNode(currentNode->left);
    if (post != NULL)
        return post;
    post = FindAChildlessNode(currentNode->right);
    return post;
}

template<class T>
void TreeRB<T>::AddComponent(T value)
{
    //  cout << "Add a " << value << endl;
    TreeNode<T> *newChild = new TreeNode<T>(value);
    if (root == NULL){
        root = newChild;
        return;
    }
    TreeNode<T>* free = FindAChildlessNode(root);
    //cout << "Free is now "; free->TraceInfo(10); cout << endl;
    free->SetChild(newChild);
    insertCase1(newChild);
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
    TreeNode<T> *pivot = n->left;

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
    cout << "case1" << endl;
    if (n->parent == NULL)
        n->color = 'b';
    else
        insertCase2(n);
}
template<class T>
void TreeRB<T>::insertCase2(TreeNode<T> *n)
{
    cout << "case2" << endl;
    if (n->parent->color == 'b')
        return;
    insertCase3(n);
}

template<class T>
void TreeRB<T>::insertCase3(TreeNode<T> *n)
{
    cout << "case3" << endl;
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
    cout << "case4" << endl;
    TreeNode<T>* g = grandParent(n);
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
    cout << "case5" << endl;
    TreeNode<T> *g = grandParent(n);
    n->parent->color = 'b';
    g->color = 'r';
    if (n == n->parent->left && n->parent == g->left)
        rotateRight(g);
    else
        rotateLeft(g);
}

template<class T>
void TreeRB<T>::deleteOneChild(TreeNode<T> *n)
{
    if (n == NULL || !(n->left == NULL && n->right == NULL))
        return;
    TreeNode<T>* child = (n->right == NULL)? n->left : n->right;

    *n = *child;
    if (n->color == 'b'){
        if (child->color == 'r')
            child->color = 'b';
        else
            deleteCase1(child);
    }

    delete n;
}


template<class T>
void TreeRB<T>::deleteCase1(TreeNode<T> *n)
{
    if (n->parent != NULL)
        deleteCase2(n);
}
template<class T>
void TreeRB<T>::deleteCase2(TreeNode<T> *n)
{
    TreeNode<T>*s = subling(n);
    if (s->color == 'r'){
        n->parent->color = 'r';
        s->color = 'b';
        if (n == n->panret->left)
            rotateLeft(n->parent);
        else
            rotateRight(n->parent);
    }
    deleteCase3(n);
}

template<class T>
void TreeRB<T>::deleteCase3(TreeNode<T> *n)
{
    TreeNode<T>*s = subling(n);
    if (n->parent->color == 'b'
        && s->color == 'b'
        && s->left->color == 'b'
        && s->right->color == 'b'){
        s->color = 'r';
        deleteCase1(n->parent);
    }else
        deleteCase4(n);
}

template<class T>
void TreeRB<T>::deleteCase4(TreeNode<T> *n)
{
    TreeNode<T>*s = subling(n);
    if (n->parent->color == 'r'
        && s->color == 'b'
        && s->left->color == 'b'
        && s->right->color == 'b'){
        s->color = 'r';
        n->parent->color = 'b';
    }else
        deleteCase5(n);
}

template<class T>
void TreeRB<T>::deleteCase5(TreeNode<T> *n)
{
    TreeNode<T>*s = subling(n);
    if (s->color == 'b'){
        if (n == n->parent->left && s->right->color == 'b' && s->left->color == 'r')
        {
            s->color = 'r';
            s->left->color = 'b';
            rotateRight(s);
        }else
            if (n == n->parent->right && s->left->color == 'b' && s->right->color == 'r'){
                s->color = 'r';
                s->right->color = 'b';
                rotateLeft(s);
            }
    }
    deleteCase6(n);
}

template<class T>
void TreeRB<T>::deleteCase6(TreeNode<T> *n)
{
    TreeNode<T>*s = subling(n);

    s->color = n->parent->color;
    n->parent->color = 'b';

    if (n == n->parent->left){
        s->right->color = 'b';
        rotateLeft(n->parent);
    }else{
        s->left->color = 'b';
        rotateRight(n->parent);
    }
}
