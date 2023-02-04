using System;
using UnityEngine;

public class CharacterManager : BasePlayerManager<CharacterControls>
{
    private void Start()
    {
        BindControls();
    }
}