import React, { ReactNode, useState, useEffect, useRef } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { UserRole } from '../types';

interface LayoutProps {
  children: ReactNode;
}

const isAdmin = (role: number | undefined): boolean => {
    return role === UserRole.Admin;
  };

const Layout: React.FC<LayoutProps> = ({ children }) => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [showUserMenu, setShowUserMenu] = useState<boolean>(false);
  const userMenuRef = useRef<HTMLDivElement>(null);

  // Close dropdown when clicking outside
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (userMenuRef.current && !userMenuRef.current.contains(event.target as Node)) {
        setShowUserMenu(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  // Debug logging
  console.log('Layout - Current user:', user);
  console.log('Layout - User role:', user?.role);
  console.log('Layout - Is admin:', isAdmin(user?.role));

  const handleLogout = async (): Promise<void> => {
    await logout();
    navigate('/login');
  };

  const getRoleColor = (role: number | undefined): string => {
    if (role === undefined) return 'bg-gray-100 text-gray-800';
    
    const colors: Record<number, string> = {
      [UserRole.Admin]: 'bg-red-100 text-red-800',
      [UserRole.Requester]: 'bg-blue-100 text-blue-800',
      [UserRole.Approver]: 'bg-green-100 text-green-800',
      [UserRole.Receiver]: 'bg-yellow-100 text-yellow-800',
      [UserRole.Purchase]: 'bg-purple-100 text-purple-800',
      [UserRole.User]: 'bg-gray-100 text-gray-800',
    };
    
    return colors[role] || 'bg-gray-100 text-gray-800';
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
    <div className="min-h-screen bg-gray-50">
      <nav className="bg-white shadow-lg">
        <div className="max-w-7xl mx-auto px-4">
          <div className="flex justify-between h-16">
            <div className="flex items-center">
              <Link to="/" className="flex-shrink-0">
                <h1 className="text-xl font-bold text-gray-800">ShoppingManager</h1>
              </Link>
              <div className="ml-6 flex space-x-8">
                <Link
                  to="/"
                  className="text-gray-500 hover:text-gray-700 px-3 py-2 rounded-md text-sm font-medium border border-blue-500"
                >
                  Dashboard (Always Visible)
                </Link>
                {isAdmin(user?.role) && (
                  <Link
                    to="/admin"
                    className="text-gray-500 hover:text-gray-700 px-3 py-2 rounded-md text-sm font-medium border border-green-500"
                  >
                    Admin Panel (Role Check)
                  </Link>
                )}
                <Link
                  to="/change-password"
                  className="text-gray-500 hover:text-gray-700 px-3 py-2 rounded-md text-sm font-medium"
                >
                  Change Password
                </Link>
                <Link
                  to="/debug"
                  className="text-gray-500 hover:text-gray-700 px-3 py-2 rounded-md text-sm font-medium border border-red-500"
                >
                  Debug
                </Link>
              </div>
            </div>
            <div className="flex items-center space-x-4">
              <span className={`px-2 py-1 rounded-full text-xs font-medium ${getRoleColor(user?.role)}`}>
                {getRoleName(user?.role)}
              </span>
              
              {/* User Menu Dropdown */}
              <div className="relative" ref={userMenuRef}>
                <button
                  onClick={() => setShowUserMenu(!showUserMenu)}
                  className="flex items-center text-gray-700 text-sm hover:text-gray-900 focus:outline-none"
                >
                  <span className="mr-2">
                    {user?.firstName} {user?.lastName}
                  </span>
                  <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 9l-7 7-7-7" />
                  </svg>
                </button>
                
                {showUserMenu && (
                  <div className="absolute right-0 mt-2 w-48 bg-white rounded-md shadow-lg py-1 z-50">
                    <Link
                      to="/change-password"
                      className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
                      onClick={() => setShowUserMenu(false)}
                    >
                      Change Password
                    </Link>
                    <Link
                      to="/profile"
                      className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
                      onClick={() => setShowUserMenu(false)}
                    >
                      View Profile
                    </Link>
                    <hr className="my-1" />
                    <button
                      onClick={handleLogout}
                      className="block w-full text-left px-4 py-2 text-sm text-red-700 hover:bg-red-50"
                    >
                      Logout
                    </button>
                  </div>
                )}
              </div>
            </div>
          </div>
        </div>
      </nav>
      <main className="max-w-7xl mx-auto py-6 px-4">
        {children}
      </main>
    </div>
  );
};

export default Layout;