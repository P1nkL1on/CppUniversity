#ifndef BASETRACK_H
#define BASETRACK_H


class BaseTrack
{
    int productCount[10];
    int totalMass;
public:
    BaseTrack();
    virtual int massLimit () = 0;
    virtual bool addProduct (const int typ, const int count){
        if (typ < 0 || typ >= 10)
            return false;
        if (totalMass >= massLimit() + count)
            return false;
        totalMass += count;
        productCount[typ]+= count;
    }

    virtual int dropProductType (const int requiredType, const int requiredCount){
        if (requiredType < 0 || requiredType >= 10)
            return 0;
        int productOfType = productCount[requiredType];

        int given = (requiredCount > productOfType)? productOfType : requiredCount;
        productOfType[requiredType] -= given;

        return given;
    }

};

class SmallTrack : public BaseTrack
{
public:
    int massLimit() override{
        return 100;
    }
};

class BigTrack : public BaseTrack
{
public:
    int massLimit() override{
        return 200;
    }
};

#endif // BASETRACK_H
