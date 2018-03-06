#include <QCoreApplication>
#include "treerb.h"

int main(int argc, char *argv[])
{
    TreeNode<float> *n0 = new TreeNode<float>(0, 16.77),
                    *n1 = new TreeNode<float>(1, 13.44),
                    *n2 = new TreeNode<float>(0, -5.33);
    n0->SetChild(n1);
    n0->SetChild(n2);

    int countNodes = n0->TraceInfo(0);
    cout << "Total nodes: " << countNodes << ';' << endl ;

    return 0;
}
