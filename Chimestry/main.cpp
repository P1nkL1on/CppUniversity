#include "molecule.h"

using namespace std;

int main(int argc, char *argv[])
{
    Atom a = Atom("aa", 3, 2);
    a.Trace();

    Molecule H2O = Molecule("water", 3, new Atom*[3]{new Atom("H ", 1, 1), new Atom("O ", 16, 8), new Atom("H ", 1, 1)});
    H2O.Trace();

    return 0;
//    Molecule H2O = Molecule("water", 3, new Atom[3] {Atom("H ", 1, 1), Atom("O ", 16, 8), Atom("H ", 1, 1)});
//    H2O.Trace();

//    return 0;
}
