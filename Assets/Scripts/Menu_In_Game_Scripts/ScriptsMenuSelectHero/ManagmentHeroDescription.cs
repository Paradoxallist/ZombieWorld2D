using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagmentHeroDescription : MonoBehaviour
{
    [SerializeField]
    private DescriptionHeroes descriptionHeroes;

    [SerializeField]
    private TMP_Text textHeroName;
    [SerializeField]
    private Image imageHero;
    [SerializeField]
    private TMP_Text textHeroDescription;
    [SerializeField]
    private TMP_Text textSkillName;
    [SerializeField]
    private Image imageSkill;
    [SerializeField]
    private TMP_Text textSkillDescription;

    private int numHero;

    public SpawnPlayer spawnPlayer;


    void Start()
    {
        numHero = 0;
        PushHero(0);
    }

    void Update()
    {
        
    }

    public void PushHero(int NumHero)
    {
        spawnPlayer.SetNumClass(NumHero);
        numHero = NumHero;
        textHeroName.text = descriptionHeroes.Description[NumHero].Name;
        imageHero.sprite = descriptionHeroes.Description[NumHero].ImageHero;
        textHeroDescription.text = descriptionHeroes.Description[NumHero].HeroDescription;
        PushButtonMainAttack();
    }

    public void PushButtonMainAttack()
    {
        textSkillName.text = descriptionHeroes.Description[numHero].NameMainAttack;
        imageSkill.sprite = descriptionHeroes.Description[numHero].ImageMainAttack;
        textSkillDescription.text = descriptionHeroes.Description[numHero].MainAttackDescription;
    }

    public void PushButtonFirstSkill()
    {
        textSkillName.text = descriptionHeroes.Description[numHero].NameFirstSkill;
        imageSkill.sprite = descriptionHeroes.Description[numHero].ImageFirstSkill;
        textSkillDescription.text = descriptionHeroes.Description[numHero].DescriptionFistSkill;
    }

    public void PushButtonSecondSkill()
    {
        textSkillName.text = descriptionHeroes.Description[numHero].NameSecondSkill;
        imageSkill.sprite = descriptionHeroes.Description[numHero].ImageSecondSkill;
        textSkillDescription.text = descriptionHeroes.Description[numHero].DescriptionSecondSkill;
    }
}
