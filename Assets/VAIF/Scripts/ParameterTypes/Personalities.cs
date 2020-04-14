using UnityEngine;


public enum Personality
{
    //https://www.16personalities.com/personality-types
    Architect, Logician, Commander, Debater,
    Advocate, Mediator, Protagonist, Campaigner,
    Logistician, Defender, Executive, Consul,
    Viruoso, Adventurer, Entrepreneur, Entertainer
}
public class Personalities : MonoBehaviour
{
    public Personality personality;
    /* Analyst personality types */
    //architect: intj
    //logician: intp
    //commander: entj
    //debater: entp

    /* Diplomat personality types */
    //advocate: infj
    //mediator: infp
    //protagonist: enfj
    //campaigner: enfp
    
    /* Sentinel personality types */
    //logistician: istj
    //defender: isfj
    //executive: estj
    //consul: esfj

    /* Explorer personality types */
    //virtuoso: istp
    //adventurer: isfp
    //entrepreneur: estp
    //entertainer: esfp
}
