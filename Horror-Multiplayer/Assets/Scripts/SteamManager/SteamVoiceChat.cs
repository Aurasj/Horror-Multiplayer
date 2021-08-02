using MLAPI;
using MLAPI.Messaging;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class SteamVoiceChat : NetworkBehaviour
{
    public static SteamVoiceChat instance;

    [SerializeField] private AudioSource audioSource;
    private bool isTalk;

    public override void NetworkStart()
    {
        if (IsLocalPlayer)
        {
            instance = this;
        }
    }

    public void PushToTalk(InputAction.CallbackContext context)
    {
        if (IsLocalPlayer)
        {
            if (context.started)
            {
                isTalk = true;
                InactiveItems.instance.pushToTalkIcon.SetActive(true);
            }
            else if (context.canceled)
            {
                isTalk = false;
                InactiveItems.instance.pushToTalkIcon.SetActive(false);
            }
        }
    }
  
    void Update()
    {
        if (IsLocalPlayer)
        {
            EVoiceResult voiceResult = SteamUser.GetAvailableVoice(out uint compressed);
            //Debug.LogFormat("MultiplayerDemoPlayer:Update - voiceResult={0}, compressed={1}", voiceResult, compressed);
            if (voiceResult == EVoiceResult.k_EVoiceResultOK && compressed > 1024)
            {
                byte[] byteBuffer = new byte[1024];
                voiceResult = SteamUser.GetVoice(true, byteBuffer, 1024, out uint bufferSize);
                if (voiceResult == EVoiceResult.k_EVoiceResultOK && bufferSize > 0)
                {
                    SendVoiceDataServerRpc(byteBuffer, bufferSize);
                }
            }
        }

            if (isTalk)
            {
                SteamUser.StartVoiceRecording();
            }
            else
            {
                SteamUser.StopVoiceRecording();
            }
    }

    [ServerRpc]
    void SendVoiceDataServerRpc(byte[] byteBuffer, uint byteCount)
    {
        //Debug.LogFormat("MultiplayerDemoPlayer:SendVoiceData - destBuffer.Length={0}, byteCount={1}", byteBuffer.Length, byteCount);
        var colliders = Physics.OverlapSphere(transform.position, 50, LayerMask.GetMask(new string[] { "Player" }));
        foreach (var collider in colliders)
        {
            var networkedObject = collider.GetComponent<NetworkObject>();
            if (networkedObject.OwnerClientId == GetComponent<NetworkObject>().OwnerClientId)
            { // Do not play voice on the player's own client
                continue;
            }
            if (networkedObject != null)
            {
                PlaySoundClientRpc(byteBuffer, byteCount,
                    new ClientRpcParams
                    {
                        Send = new ClientRpcSendParams
                        {
                            TargetClientIds = new ulong[] { networkedObject.OwnerClientId }
                        }
                    }
                );
            }
        }
    }

    [ClientRpc]
    void PlaySoundClientRpc(byte[] byteBuffer, uint byteCount, ClientRpcParams clientRpcParams = default)
    {
        //Debug.LogFormat("MultiplayerDemoPlayer:ClientPlaySound - destBuffer.Length={0}, byteCount={1}", byteBuffer.Length, byteCount);
        byte[] destBuffer = new byte[22050 * 2];
        EVoiceResult voiceResult = SteamUser.DecompressVoice(byteBuffer, byteCount, destBuffer, (uint)destBuffer.Length, out uint bytesWritten, 22050);
        //Debug.LogFormat("MultiplayerDemoPlayer:ClientPlaySound - voiceResult={0}, bytesWritten={1}", voiceResult, bytesWritten);
        if (voiceResult == EVoiceResult.k_EVoiceResultOK && bytesWritten > 0)
        {
            audioSource.clip = AudioClip.Create(UnityEngine.Random.Range(100, 1000000).ToString(), 22050, 1, 22050, false);
            float[] test = new float[22050];
            for (int i = 0; i < test.Length; ++i)
            {
                test[i] = (short)(destBuffer[i * 2] | destBuffer[i * 2 + 1] << 8) / 32768.0f;
            }
            audioSource.clip.SetData(test, 0);
            audioSource.Play();
        }
    }

}
