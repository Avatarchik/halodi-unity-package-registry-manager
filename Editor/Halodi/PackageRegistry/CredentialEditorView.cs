
using System;
using UnityEditor;
using UnityEngine;

namespace Halodi.PackageRegistry
{
    class CredentialEditorView : EditorWindow
    {
        private bool initialized = false;

        private CredentialManager credentialManager;

        private bool createNew;

        private ScopedRegistry registry;

        void OnEnable()
        {
        }

        void OnDisable()
        {
            initialized = false;
        }

        public void CreateNew(CredentialManager credentialManager)
        {
            this.credentialManager = credentialManager;
            this.createNew = true;
            this.registry = new ScopedRegistry();
            this.initialized = true;
        }

        public void Edit(NPMCredential credential, CredentialManager credentialManager)
        {
            this.credentialManager = credentialManager;
            this.registry = new ScopedRegistry();
            this.registry.url = credential.url;
            this.registry.auth = credential.alwaysAuth;
            this.registry.token = credential.token;

            this.createNew = false;
            this.initialized = true;
        }


        void OnGUI()
        {
            if (initialized)
            {


                if (createNew)
                {
                    EditorGUILayout.LabelField("Add credential ");

                    registry.url = EditorGUILayout.TextField("url: ", registry.url);
                }
                else
                {
                    EditorGUILayout.LabelField("Edit credential");
                    EditorGUILayout.LabelField("url: " + registry.url);
                }

                registry.auth = EditorGUILayout.Toggle("Always auth: ", registry.auth);
                registry.token = EditorGUILayout.TextField("Token: ", registry.token);



                if (GUILayout.Button("Login to registry and get token"))
                {
                    GetTokenView.CreateWindow(registry);
                }

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Tip: Restart Unity to reload credentials after saving.");


                if (createNew)
                {
                    if (GUILayout.Button("Add"))
                    {
                        Save();
                    }
                }
                else
                {
                    if (GUILayout.Button("Save"))
                    {
                        Save();
                    }
                }


                if (GUILayout.Button("Cancel"))
                {
                    Close();
                    GUIUtility.ExitGUI();
                }
            }
        }

        private void Save()
        {
            if (registry.isValidCredential() && !string.IsNullOrEmpty(registry.token))
            {
                credentialManager.SetCredential(registry.url, registry.auth, registry.token);
                credentialManager.Write();
                Close();
                GUIUtility.ExitGUI();
            }
            else
            {
                EditorUtility.DisplayDialog("Invalid", "Invalid settings for credential.", "Ok");
            }
        }



    }
}
