#ifndef PARTICLE_H
#define PARTICLE_H

#include "cstdlib"
#include "iostream"
#include <stdio.h>
using namespace std;

class particle
{
public:
    Proton();
    virtual int getMass () const {return 0;}
    virtual char* getName() const {return "0";}
    virtual int getQ() const {return 0;}
    virtual void Trace () const {cout << getName();}
};


class Proton : public particle
{
public:
    Proton();
    int getQ () const {return 1;}
    int getMass () const {return 1;}
    char* getName() const {return "p+";}
};

class Neitron : public particle
{
public:
    Neitron();
    int getMass () const {return 1;}
    int getQ () const {return 0;}
    char* getName() const {return "n0";}
};

class Electron : public particle
{
public:
    Electron();
    int getMass () const {return 0;}
    int getQ () const {return -1;}
    char* getName() const {return "e-";}
};

class BigParticle
{
public:
    virtual int getElectronCount () const  = 0;
    virtual int getProtonCount () const  = 0;
    virtual int getNeironCount () const  = 0;
};

#endif // PARTICLE_H
