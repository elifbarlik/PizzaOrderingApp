(function(){
  const DB_NAME = 'pitzam-db';
  const STORE = 'kv';
  const DB_VERSION = 1;

  function openDb(){
    return new Promise((resolve, reject) => {
      const req = indexedDB.open(DB_NAME, DB_VERSION);
      req.onupgradeneeded = () => {
        const db = req.result;
        if(!db.objectStoreNames.contains(STORE)){
          db.createObjectStore(STORE);
        }
      };
      req.onsuccess = () => resolve(req.result);
      req.onerror = () => reject(req.error);
    });
  }

  async function withStore(mode, cb){
    const db = await openDb();
    return new Promise((resolve, reject) => {
      const tx = db.transaction(STORE, mode);
      const store = tx.objectStore(STORE);
      const result = cb(store);
      tx.oncomplete = () => resolve(result);
      tx.onerror = () => reject(tx.error);
    });
  }

  async function set(key, value){
    await withStore('readwrite', store => store.put(value, key));
  }

  async function get(key){
    return await withStore('readonly', store => new Promise((resolve, reject) => {
      const req = store.get(key);
      req.onsuccess = () => resolve(req.result || null);
      req.onerror = () => reject(req.error);
    }));
  }

  async function remove(key){
    await withStore('readwrite', store => store.delete(key));
  }

  async function migrateFromLocalStorage(prefixes){
    try{
      const prefs = Array.isArray(prefixes) ? prefixes : [];
      for(let i=0;i<localStorage.length;i++){
        const key = localStorage.key(i);
        if(!key) continue;
        if(prefs.length>0 && !prefs.some(p=> key.startsWith(p))) continue;
        const existing = await get(key);
        if(existing != null) continue; // already migrated
        const val = localStorage.getItem(key);
        if(val != null){
          await set(key, val);
        }
      }
    }catch(e){
      // ignore
    }
  }

  window.pitzamIdb = { set, get, remove, migrateFromLocalStorage };
})();


