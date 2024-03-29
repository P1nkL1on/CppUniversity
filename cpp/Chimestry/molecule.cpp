#include "molecule.h"

Molecule::Molecule()
{
    name = "none";
    atomCount = 0;
}

Molecule::Molecule(char* molName, int molAtomCount, Atom** molAtoms)
{
    cout << "+ " << molName << endl;
    atoms = molAtoms;
    atomCount = molAtomCount;
    name = molName;
}

Molecule::~Molecule()
{
    delete[] atoms;
    cout << "~ " << name << endl;
}

void Molecule::Trace() const{
    int ac = -1;

    cout << name  << endl << "  Mass: " << getMass() <<  endl << "  e- : " << getElectronCount() << endl;
    cout << "  p+ :  " << getProtonCount() << endl << "  n0 :  " << getNeironCount() << endl;
    cout << endl << "Formula: " << endl;

    char typesWas[18];

    while (atomCount - (ac++) - 1)
        if (typesWas[atoms[ac]->getElectronCount() - 1] != 'y')
        {
            typesWas[atoms[ac]->getElectronCount() - 1] = 'y';
            int count = CountAtomType(atoms[ac]->getElectronCount());
            char* name = atoms[ac]->getName();

            if (name[1] == ' ') cout << name[0]; else cout << name;
            if (count != 1)cout << count;
        }
    cout << endl << "  e-: " << getElectronCount() << endl << "  Mass: " << getMass() <<  endl << "Atoms:" << endl;
    ac = -1;
    while (atomCount - (ac++) - 1)
        if (typesWas[atoms[ac]->getElectronCount() - 1] != 'n'){
            typesWas[atoms[ac]->getElectronCount() - 1] = 'n';
            cout << CountAtomType(atoms[ac]->getName()) << " X ";
            atoms[ac]->Trace();
        }

}

int Molecule::CountAtomType (int type) const{
    return CountAtomType(atomnames[type - 1]);
}
int Molecule::CountAtomType (char* type) const{
    int ac = atomCount, summ = 0;
    while (--ac>=0)
        summ += 1 * (atoms[ac]->getName()[0] == type[0] && atoms[ac]->getName()[1] == type[1]);
    return summ;
}


int Molecule::getElectronCount () const{
    int ac = atomCount, summ = 0;
    while (--ac>=0) summ += atoms[ac]->getElectronCount();
    return summ;
}
int Molecule::getProtonCount () const{
    int ac = atomCount, summ = 0;
    while (--ac>=0) summ += atoms[ac]->getProtonCount();
    return summ;
}
int Molecule::getNeironCount () const{
    int ac = atomCount, summ = 0;
    while (--ac>=0) summ += atoms[ac]->getNeironCount();
    return summ;
}

int Molecule::getMass() const
{
    int ac = atomCount, summ = 0;
    while (--ac>=0) summ += atoms[ac]->getMass();
    return summ;
}

