using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteamChat : MonoBehaviour
{
    protected Callback<LobbyChatUpdate_t> Callback_lobbyChatUpdate;
    protected Callback<LobbyChatMsg_t> Callback_lobbyChatMsg;

    ulong current_lobbyID;
    string personaName;

    bool isChatting = false;
    string chatInput = "";
    string chatBinding;
    string voiceChatBinding;

    List<ChatMessage> chatMessages = new List<ChatMessage>();

    [System.Serializable]
    public class ChatMessage
    {
        public string sender = "";
        public string message = "";
        public float timer = 0;
    }
    void Start()
    {
        Callback_lobbyChatMsg = Callback<LobbyChatMsg_t>.Create(OnLobbyChatMsg);
        Callback_lobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);

        personaName = SteamFriends.GetPersonaName();

        current_lobbyID = Convert.ToUInt64(PlayerPrefs.GetString("current_lobbyID"));

        chatBinding = PlayerPrefs.GetString("chatbinding");
        voiceChatBinding = PlayerPrefs.GetString("voicechatbinding");
    }

    void Update()
    {
        SteamAPI.RunCallbacks();

        //Hide messages after timer is expired
        for (int i = 0; i < chatMessages.Count; i++)
        {
            if (chatMessages[i].timer > 0)
            {
                chatMessages[i].timer -= Time.deltaTime;
            }
        }
    }

    public void ChatButton()
    {
        if (!isChatting)
        {
            isChatting = true;
            chatInput = "";
        }
    }

    void OnGUI()
    {
        if (!isChatting)
        {
            GUI.Label(new Rect(5, Screen.height - 25, 200, 25), "'" + chatBinding + "' " + "to chat " + "|" + " '" + voiceChatBinding + "' " + "to voice");
        }
        else
        {
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
            {
                isChatting = false;
                if (chatInput.Replace(" ", "") != "")
                {
                    //Send message
                    SubmitChatText(chatInput);
                }
                chatInput = "";
            }

            GUI.SetNextControlName("ChatField");
            GUI.Label(new Rect(5, Screen.height - 25, 200, 25), "Say:");
            GUIStyle inputStyle = GUI.skin.GetStyle("box");
            inputStyle.alignment = TextAnchor.MiddleLeft;
            chatInput = GUI.TextField(new Rect(10 + 25, Screen.height - 27, 400, 22), chatInput, 60, inputStyle);

            GUI.FocusControl("ChatField");
        }

        //Show messages
        for (int i = 0; i < chatMessages.Count; i++)
        {
            if (chatMessages[i].timer > 0 || isChatting)
            {
                GUI.Label(new Rect(5, Screen.height - 50 - 25 * i, 500, 25), chatMessages[i].sender + ": " + chatMessages[i].message);
            }
        }
    }

    void SubmitChatText(string text)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
        Debug.Log("Chat: " + personaName + ": '" + text + "' Len: " + text.Length + " bLen: " + bytes.Length);
        SteamMatchmaking.SendLobbyChatMsg((CSteamID)current_lobbyID, bytes, bytes.Length + 1);
    }

    void OnLobbyChatUpdate(LobbyChatUpdate_t pCallback)
    {
        Debug.Log("[" + LobbyChatUpdate_t.k_iCallback + " - LobbyChatUpdate] - " + pCallback.m_ulSteamIDLobby + " -- " + pCallback.m_ulSteamIDUserChanged + " -- " + pCallback.m_ulSteamIDMakingChange + " -- " + pCallback.m_rgfChatMemberStateChange);
    }

    void OnLobbyChatMsg(LobbyChatMsg_t pCallback)
    {
        Debug.Log("[" + LobbyChatMsg_t.k_iCallback + " - LobbyChatMsg] - " + pCallback.m_ulSteamIDLobby + " -- " + pCallback.m_ulSteamIDUser + " -- " + pCallback.m_eChatEntryType + " -- " + pCallback.m_iChatID);

        CSteamID SteamIDUser;
        byte[] Data = new byte[4096];
        EChatEntryType ChatEntryType;
        int ret = SteamMatchmaking.GetLobbyChatEntry((CSteamID)pCallback.m_ulSteamIDLobby, (int)pCallback.m_iChatID, out SteamIDUser, Data, Data.Length, out ChatEntryType);

        Debug.Log("SteamMatchmaking.GetLobbyChatEntry(" + (CSteamID)pCallback.m_ulSteamIDLobby + ", " + (int)pCallback.m_iChatID + ", out SteamIDUser, Data, Data.Length, out ChatEntryType) : " + ret + " -- " + SteamIDUser + " -- " + System.Text.Encoding.UTF8.GetString(Data) + " -- " + ChatEntryType);

        string data = System.Text.Encoding.UTF8.GetString(Data);

        ChatMessage m = new ChatMessage();
        m.sender = SteamFriends.GetFriendPersonaName(SteamIDUser);
        m.message = data;
        m.timer = 15.0f;

        chatMessages.Insert(0, m);
        if (chatMessages.Count > 8)
        {
            chatMessages.RemoveAt(chatMessages.Count - 1);
        }
    }
}
