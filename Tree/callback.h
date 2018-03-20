#ifndef CALLBACK_H
#define CALLBACK_H

#include "treenode.h"

class CallBack
{
public:
    CallBack();

    template <class T>
    operator ()(TreeNode<T> *n){
        if (!n){
            cout << "None" << endl;
            return 0;
        }
        TreeNode<T>*p = n;
        while (p->parent)
            {cout << ".  "; p = p->parent;}
        cout << n->getDate() << endl;
    }
};

#endif // CALLBACK_H
