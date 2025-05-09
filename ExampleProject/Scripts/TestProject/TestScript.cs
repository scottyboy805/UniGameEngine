class Example
{
#if UNIGAME_EDITOR
    public bool val1;
#endif

#if UNIGAME
    public bool val2;
#endif
}