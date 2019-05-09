using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerNetworkMover : Photon.MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 position;
    Quaternion rotation;
    float smoothing = 10f;

    void Start()
    {

        if (photonView.isMine)
        {
            Debug.Log(photonView.isMine);
            ///自分のキャラ以外はコンポーネントをオフのままにします。
            GetComponent<FirstPersonController>().enabled = true;
            GetComponent<CharacterController>().enabled = true;
            GetComponent<AudioSource>().enabled = true;
            foreach (Camera cam in GetComponentsInChildren<Camera>())
                cam.enabled = true;
        }
        else
        {
            StartCoroutine("UpdateData");//コルーチンを始めます。
        }
    }
    ///場所の同期を取った時に間を補完する処理（スムーズに動く）。
    IEnumerator UpdateData()
    {
        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * smoothing);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smoothing);
            yield return null;
        }
    }


    ///場所の同期を取る処理。
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
           
        }
        else
        {
            position = (Vector3)stream.ReceiveNext();
            rotation = (Quaternion)stream.ReceiveNext();
          
        }
    }
}
