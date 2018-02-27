#include "atom.h"

int Atom::getElectronCount() const
{
    particle** p = particles;
    int summ = 0, now = 0;
    while (p && now ++ < particlecount)
        summ += ((*p++)->getQ() == -1) * 1;
    return summ;
}

int Atom::getProtonCount() const
{
    int pc = particlecount, summ = 0;
    while (--pc >= 0) summ += (particles[pc]->getQ() == 1 && particles[pc]->getMass() == 1) * 1;
    return summ;
}

int Atom::getNeironCount() const
{
    int pc = particlecount, summ = 0;
    while (--pc >= 0) summ += (particles[pc]->getQ() == 0) * 1;
    return summ;
}

int Atom::getMass() const
{
    int pc = particlecount, summ = 0;
    while (--pc >= 0) summ += (particles[pc]->getMass());
    return summ;
}

Atom::Atom() : BigParticle()
{
    name = "--";
    number = 0;
    particlecount = 1;
    particles = new particle* [1];
    particles[0] = new Electron();
}

Atom::Atom(char *atomName, int atomMass, int atomQ) : BigParticle()
{
    cout  << "+ " << atomName<< endl;
    name = atomName;
    number = atomQ;

    particles = new particle*[atomMass + atomQ];

    for (int i = 0; i < atomMass - atomQ; i++)
        particles[i] = new Neitron();

    for (int i = atomMass - atomQ; i < atomMass + atomQ; i+= 2){
        particles[i] = new Electron();
        particles[i + 1] = new Proton();
    }


    particlecount = atomMass + atomQ;

}


void Atom::Trace() const
{
    cout << name  << "  ( #" << number << " )"<< endl << "  Mass: " << getMass() <<  endl << "  e- : " << getElectronCount() << endl;
    cout << "  p+ :  " << getProtonCount() << endl << "  n0 :  " << getNeironCount() << endl;

    particle** p = particles;
    int now = 0;
    while (p && now ++ < particlecount)
        (*p++)->Trace();
    cout << endl;
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
