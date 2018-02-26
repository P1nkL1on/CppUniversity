#include "atom.h"

int Atom::getElectronCount() const
{
    int pc = particlecount, summ = 0;
    while (pc--) summ += (particles[pc].getQ() == -1) * 1;
    return summ;
}

int Atom::getProtonCount() const
{
    int pc = particlecount, summ = 0;
    while (pc--) summ += (particles[pc].getQ() == 1 && particles[pc].getMass() == 1) * 1;
    return summ;
}

int Atom::getNeironCount() const
{
    int pc = particlecount, summ = 0;
    while (pc--) summ += (particles[pc].getQ() == 0) * 1;
    return summ;
}

int Atom::getMass() const
{
    int pc = particlecount, summ = 0;
    while (pc--) summ += (particles[pc].getMass());
    return summ;
}

Atom::Atom()
{
    name = "--";
    number = 0;
    particlecount = 1;
    particles = new particle[1];
    particles[0] = Electron();
}

Atom::Atom(char *atomName, int atomMass, int atomQ)
{
    cout  << "+ " << atomName<< endl;
    name = atomName;
    number = atomQ;

    particles = new particle[atomMass + atomQ];

    for (int i = 0; i < atomQ; i++){
        particles[i * 2] = Electron();
        particles[i * 2 + 1] = Proton();
    }
    for (int i = atomQ; i < atomMass; i++)
        particles[i] = Neitron();

    particlecount = atomMass + atomQ;

}


void Atom::Trace() const
{
    cout << name  << "  ( #" << number << " )"<< endl << "  Mass: " << getMass() <<  endl << "  e- : " << getElectronCount() << endl;
    cout << "  p+ :  " << getProtonCount() << endl << "  n0 :  " << getNeironCount() << endl;

    int pc = particlecount;
    while (pc--) particles[pc].Trace();
}

Atom::~Atom()
{
    cout << "~ " << name << endl;
    delete [] particles;
}

char *Atom::getName() const
{
    return name;
}
