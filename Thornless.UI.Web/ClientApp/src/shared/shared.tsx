export default class SharedMethods
{
    static IsNullOrEmpty(object: any | undefined) : boolean
    {
        return (typeof object === 'undefined' || !object);
    }
}