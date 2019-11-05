namespace IPR.AstrandTest
{
    public enum AstrandTestPhase
    {
        WARMING_UP,
        MAIN_TEST,
        COOLING_DOWN,

        EXTENDED_TEST,  //In case the main test needs to be extended
        INACTIVE        //No test is active
    }
}