#ifndef PARTICLE_H
#define PARTICLE_H

#include "cstdlib"
#include "iostream"
#include <stdio.h>
using namespace std;

class particle
{
public:
    particle(){}
    virtual int getMass () const {return 0;}
    virtual char* getName() const {return "0";}
    virtual int getQ() const {return 0;}
    virtual void Trace () const {cout << getName();}
};


class Proton : public particle
{
public:
    Proton() : particle(){}
    int getQ () const override {return 1;}
    int getMass () const override {return 1;}
    char* getName() const override {return "p+";}
};

class Neitron : public particle
{
public:
    Neitron() : particle(){}
    int getMass () const override {return 1;}
    int getQ () const override {return 0;}
    char* getName() const override {return "n0";}
};

class Electron : public particle
{
public:
    Electron() : particle(){}
    int getMass () const override {return 0;}
    int getQ () const override {return -1;}
    char* getName() const override {return "e-";}
};

class BigParticle : public particle
{
public:
    BigParticle() {}
    virtual int getElectronCount () const  = 0;
    virtual int getProtonCount () const  = 0;
    virtual int getNeironCount () const  = 0;
};

#endif // PARTICLE_H
