#include <QCoreApplication>
#include "treerb.h"
#include "treerb.cpp"
#include "treenode.cpp"

int main(int argc, char *argv[])
{
    TreeRB<int> t;
    for (int i = 0; i < 5; i++, t.Trace(1), cout << endl << endl << i << endl)
    {
        t.AddComponent(i);
        char* X;
        cin >> X;
    }
    //t.insertCase1(new TreeNode<int>('b', 8));
    //t.Trace(1);
    return 0;
}
