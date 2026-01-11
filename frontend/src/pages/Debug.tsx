import React from 'react';
import { useAuth } from '../context/AuthContext';
import { UserRole } from '../types';
import Layout from '../components/Layout';

const Debug: React.FC = () => {
  const { user, isAuthenticated } = useAuth();

  const isAdmin = (role: number | undefined): boolean => {
    return role === UserRole.Admin;
  };

  const getRoleName = (role: number | undefined): string => {
    if (role === undefined) return 'Unknown';
    
    const roleNames: Record<number, string> = {
      [UserRole.User]: 'User',
      [UserRole.Admin]: 'Admin',
      [UserRole.Requester]: 'Requester',
      [UserRole.Approver]: 'Approver',
      [UserRole.Receiver]: 'Receiver',
      [UserRole.Purchase]: 'Purchase',
    };
    
    return roleNames[role] || 'Unknown';
  };

  return (
    <Layout>
      <div className="space-y-6">
        <h1 className="text-2xl font-bold text-gray-900">Debug Information</h1>
        
        <div className="bg-white shadow overflow-hidden sm:rounded-lg">
          <div className="px-4 py-5 sm:p-6">
            <h3 className="text-lg leading-6 font-medium text-gray-900 mb-4">
              Authentication Status
            </h3>
            <dl className="space-y-3">
              <div>
                <dt className="text-sm font-medium text-gray-500">Is Authenticated</dt>
                <dd className="text-sm text-gray-900">{isAuthenticated ? 'Yes' : 'No'}</dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-gray-500">User Object</dt>
                <dd className="text-sm text-gray-900">
                  <pre className="bg-gray-100 p-2 rounded text-xs overflow-auto">
                    {JSON.stringify(user, null, 2)}
                  </pre>
                </dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-gray-500">User Role (Raw)</dt>
                <dd className="text-sm text-gray-900">{user?.role !== undefined ? user.role : 'No role'}</dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-gray-500">User Role (Name)</dt>
                <dd className="text-sm text-gray-900">{getRoleName(user?.role)}</dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-gray-500">Role Type</dt>
                <dd className="text-sm text-gray-900">{typeof user?.role}</dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-gray-500">Is Admin Check</dt>
                <dd className="text-sm text-gray-900">{isAdmin(user?.role) ? 'Yes' : 'No'}</dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-gray-500">UserRole.Admin Value</dt>
                <dd className="text-sm text-gray-900">{UserRole.Admin}</dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-gray-500">Role === UserRole.Admin</dt>
                <dd className="text-sm text-gray-900">{(user?.role === UserRole.Admin) ? 'Yes' : 'No'}</dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-gray-500">Role === 1 (Admin)</dt>
                <dd className="text-sm text-gray-900">{(user?.role === 1) ? 'Yes' : 'No'}</dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-gray-500">UserRole Enum Values</dt>
                <dd className="text-sm text-gray-900">
                  <pre className="bg-gray-100 p-2 rounded text-xs">
                    {JSON.stringify(UserRole, null, 2)}
                  </pre>
                </dd>
              </div>
            </dl>
          </div>
        </div>
        
        <div className="bg-white shadow overflow-hidden sm:rounded-lg">
          <div className="px-4 py-5 sm:p-6">
            <h3 className="text-lg leading-6 font-medium text-gray-900 mb-4">
              Local Storage
            </h3>
            <dl className="space-y-3">
              <div>
                <dt className="text-sm font-medium text-gray-500">Token</dt>
                <dd className="text-sm text-gray-900 break-all">
                  {localStorage.getItem('token') || 'No token'}
                </dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-gray-500">User Data</dt>
                <dd className="text-sm text-gray-900">
                  <pre className="bg-gray-100 p-2 rounded text-xs overflow-auto">
                    {localStorage.getItem('user') || 'No user data'}
                  </pre>
                </dd>
              </div>
            </dl>
          </div>
        </div>
      </div>
    </Layout>
  );
};

export default Debug;