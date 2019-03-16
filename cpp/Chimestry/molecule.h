#ifndef MOLECULE_H
#define MOLECULE_H

#include "atom.h"

class Molecule : public BigParticle
{
private:
    char* name;
    int atomCount;
    Atom** atoms;
public:
    Molecule();
    Molecule (char *molName, int molAtomCount, Atom **molAtoms);
    ~Molecule();
    void Trace() const;

    int CountAtomType (int type) const;
    int CountAtomType (char* type) const;

    int getElectronCount () const;
    int getProtonCount () const;
    int getNeironCount () const;
    int getMass () const;
};

#endif // MOLECULE_H
