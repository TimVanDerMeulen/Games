using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBroadcaster : MonoBehaviour
{
	
	private List<Subscriber> subscribers = new List<Subscriber>();
	
    /*
	########################## Game Loop ##########################
	*/
	
	public void Start() {
		//subscribers = new List<Subscriber>();
    }
	
	public void OnCollisionEnter(Collision collision){
		foreach (Subscriber subscriber in subscribers){
			subscriber.targetCollisionEnter(gameObject, collision);
		}
	}
	
	public void OnCollisionExit(Collision collision){
		foreach (Subscriber subscriber in subscribers){
			subscriber.targetCollisionExit(gameObject, collision);
		}
	}
	
	void OnTriggerEnter(Collider collider){
		foreach (Subscriber subscriber in subscribers){
			subscriber.targetTriggerEnter(gameObject, collider);
		}
	}
	
	void OnTriggerExit(Collider collider){
		foreach (Subscriber subscriber in subscribers){
			subscriber.targetTriggerExit(gameObject, collider);
		}
	}
	
	/*
	########################## Other ##########################
	*/
	
	public interface Subscriber{
		void targetCollisionEnter(GameObject target, Collision collision);
		void targetCollisionExit(GameObject target, Collision collision);
		void targetTriggerEnter(GameObject target, Collider collider);
		void targetTriggerExit(GameObject target, Collider collider);
	}
	
	public void subscribe(Subscriber subscriber){
		if(!subscribers.Contains(subscriber))
			subscribers.Add(subscriber);
	}
	
	public void unsubscribe(Subscriber subscriber){
		if(subscribers.Contains(subscriber))
			subscribers.Remove(subscriber);
	}
	
}
