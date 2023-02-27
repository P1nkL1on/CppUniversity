#ifndef CHIMSTRUCT_H
#define CHIMSTRUCT_H
#include "particle.h"

static char atomnames[18][3] = {"H ", "He","Li","Be","B ","C ","N ","O ","F ","Ne","Na","Mg","Al","Si","P ","S ","Cl","Ar"};
struct Atom : public BigParticle
{
private:
    particle** particles;
    char *name;
    int number, particlecount;
public:
    Atom ();
    Atom (char *atomName, int atomMass, int atomQ);
    ~Atom();

    int getElectronCount () const override;
    int getProtonCount () const override;
    int getNeironCount ()const override;
    int getMass () const override;
    char* getName() const override;
    //int getValentElectronCount () const;

    void Trace () const  override;
};





#endif // CHIMSTRUCT_H
