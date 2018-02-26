#ifndef CHIMSTRUCT_H
#define CHIMSTRUCT_H
#include "particle.h"

static char atomnames[18][3] = {"H ", "He","Li","Be","B ","C ","N ","O ","F ","Ne","Na","Mg","Al","Si","P ","S ","Cl","Ar"};
struct Atom : public BigParticle
{
private:
    particle* particles;
    char *name;
    int number, particlecount;
public:
    Atom ();
    Atom (char *atomName, int atomMass, int atomQ);
    ~Atom();

    int getElectronCount () const;
    int getProtonCount () const;
    int getNeironCount ()const;
    int getMass () const;
    char* getName() const;
    int getValentElectronCount () const;

    void Trace () const;
};





#endif // CHIMSTRUCT_H
