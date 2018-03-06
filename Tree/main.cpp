#include <QCoreApplication>
#include "treerb.h"
#include "treerb.cpp"
#include "treenode.cpp"

int main(int argc, char *argv[])
{
//    TreeNode<float> *n0 = new TreeNode<float>(0, 16.77),
//                    *n1 = new TreeNode<float>(1, 13.44),
//                    *n2 = new TreeNode<float>(0, -5.33);
//    n0->SetChild(n1);
//    n0->SetChild(n2);

//    int countNodes = n0->TraceInfo(0);
//    cout << "Total nodes: " << countNodes << ';' << endl ;
    TreeRB<int> t;
    t.insertCase1(new TreeNode<int>('b', 15));
    t.Trace(1);
    //    for (int i = 0; i < 10; i++){
//        //t.insertCase1(new TreeNode<int>('b', i * i));
//        t.Trace(1);
//    }

    return 0;
}
