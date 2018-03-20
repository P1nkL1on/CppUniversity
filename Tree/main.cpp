#include <QCoreApplication>
#include "treerb.h"
#include "treerb.cpp"
#include "treenode.cpp"
#include "treeiterator.h"
#include "treeiterator.cpp"

int main(int argc, char *argv[])
{
    TreeRB<int>* t = new TreeRB<int>();
    CallBack cb = CallBack();

    for (int i = 0; i < 20; i++, t->DeepByPass(cb), cout << endl)
        t->AddComponent(i);

    TreeIterator<int>* ti = new TreeIterator<int>(t);

    for (int i = 0; i < 10; i++){
        ti->Inc();
        cb(ti->get());
    }

//    TreeNode<int> *n = t->Find(14);
//    t->deleteOneChild(n);

//    n = t->Find(2);
//    t->deleteOneChild(n);

//    t->DeepByPass();

    delete t;
    return 0;
}
