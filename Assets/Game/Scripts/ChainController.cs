
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public class ChainController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D soulRb;
        [SerializeField] private Rigidbody2D bodyRb;
        [SerializeField] private GameObject chainLinkPrefab;

        [Header("Chain Settings")]
        [SerializeField] private int linkCount = 16;
        [SerializeField] private float linkLength = 0.25f;

        private List<Rigidbody2D> chainLinks = new List<Rigidbody2D>();

        private IEnumerator Start()
        {
            // Wait until the player has been moved to its spawn position.
            yield return null;

            GenerateChain();
        }

        private void GenerateChain()
        {
            if (soulRb == null || bodyRb == null || chainLinkPrefab == null)
            {
                Debug.LogError("ChainController is missing references!");
                return;
            }

            if (linkCount < 1 || linkLength <= 0f)
            {
                Debug.LogError("Chain link count and length must be greater than zero.");
                return;
            }

            Vector2 soulPosition = soulRb.position;
            Vector2 bodyPosition = bodyRb.position;

            Vector2 direction = (bodyPosition - soulPosition).normalized;

            // Calculate the distance between each link.
            float spacing = Vector2.Distance(soulPosition, bodyPosition) / linkCount;

            for (int i = 0; i < linkCount; i++)
            {
                // Calculate the position of this link.
                Vector2 position = soulPosition + direction * spacing * (i + 0.5f);

                // Calculate the rotation of this link.
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Create the chain link.
                GameObject linkObject = Instantiate(
                    chainLinkPrefab,
                    position,
                    Quaternion.Euler(0f, 0f, angle),
                    transform
                );

                Rigidbody2D linkRb = linkObject.GetComponent<Rigidbody2D>();

                if (linkRb == null)
                {
                    Debug.LogError("Chain link prefab needs a Rigidbody2D!");
                    Destroy(linkObject);
                    return;
                }

                chainLinks.Add(linkRb);

                // Get the HingeJoint2D on this link.
                HingeJoint2D joint = linkObject.GetComponent<HingeJoint2D>();

                if (joint == null)
                {
                    Debug.LogError("Chain link prefab needs a HingeJoint2D!");
                    Destroy(linkObject);
                    return;
                }

                // Set up the joint's local anchor.
                joint.autoConfigureConnectedAnchor = false;
                joint.anchor = new Vector2(-linkLength / 2f, 0f);
                joint.enableCollision = false;

                // Connect the first link to the Soul.
                if (i == 0)
                {
                    joint.connectedBody = soulRb;

                    Vector2 worldAnchor = soulPosition;
                    joint.connectedAnchor = soulRb.transform.InverseTransformPoint(worldAnchor);
                }
                // Connect every other link to the previous link.
                else
                {
                    Rigidbody2D previousLink = chainLinks[i - 1];

                    joint.connectedBody = previousLink;

                    Vector2 worldAnchor = previousLink.transform.TransformPoint(
                        new Vector2(linkLength / 2f, 0f)
                    );

                    joint.connectedAnchor = previousLink.transform.InverseTransformPoint(worldAnchor);
                }
            }

            // Connect the final link to the Body.
            Rigidbody2D lastLink = chainLinks[chainLinks.Count - 1];

            HingeJoint2D endJoint = lastLink.gameObject.AddComponent<HingeJoint2D>();

            endJoint.autoConfigureConnectedAnchor = false;
            endJoint.connectedBody = bodyRb;
            endJoint.anchor = new Vector2(linkLength / 2f, 0f);

            endJoint.connectedAnchor = bodyRb.transform.InverseTransformPoint(
                bodyPosition
            );

            endJoint.enableCollision = false;

            Debug.Log("Chain generated with " + linkCount + " links.");
        }
    }
}
