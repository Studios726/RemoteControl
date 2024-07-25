using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager :Singleton<GameDataManager>
{
   private ClientConnection _clientConnection;
   public ClientConnection Connection
   {
      get => _clientConnection;
      set => _clientConnection = value;
   }
}
