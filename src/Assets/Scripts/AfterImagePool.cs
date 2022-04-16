using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AfterImagePool: MonoBehaviour
{
    [SerializeField] private GameObject afterImagePrefab;

    private Queue<GameObject> _afterImagePool = new Queue<GameObject>();

    [SerializeField] private int poolSize = 10;

    public static AfterImagePool SingleTonInstance { get; private set; }
    

    private void Awake()
    {
        SingleTonInstance = this;
        grow_pool(); // pre-grow for use
    }

    /* A method to instantiate afterimages.
     * 
     * Instantiate poolSize afterimages
     * Push into pool
     * Set "inUse" as false
     */
    private void grow_pool()
    {
        for (int i = 0; i < poolSize; ++i)
        {
            var afterImage = Instantiate(afterImagePrefab);
            afterImage.transform.SetParent(transform); //
            return_to_pool(afterImage);
        }
    }

    /* A method to grab the afterimage from the pool.
     *
     * If Pool is empty, call the instantiate method (might cause FPS spike)
     * grab an afterimage and set its "inUse" as true
     * 
     * @return The afterimage
     *
     */
    public GameObject grab_from_pool()
    {
        if (!_afterImagePool.Any())
        {
            grow_pool();
        }

        var afterImage = _afterImagePool.Dequeue();
        afterImage.SetActive(true);
        return afterImage;
    }

    /* A method to return the afterimage back to the pool.
     * 
     * Push it back into the pool
     * Set its "inUse" as false
     */
    public void return_to_pool(GameObject instance)
    {
        instance.SetActive(false);
        _afterImagePool.Enqueue(instance);
    }
}