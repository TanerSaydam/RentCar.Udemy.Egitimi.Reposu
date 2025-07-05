export interface NavigationModel{
    title: string;
    url?: string;
    icon?: string;
    haveSubNav?: boolean;
    subNavs?: NavigationModel[]
}

export const navigations: NavigationModel[] = [
    {
        title: 'Dashboard',
        url: '/',
        icon: 'bi-speedometer2'
    },
    {
        title: 'Şubeler',
        url: '/branches',
        icon: 'bi-buildings'
    },
    {
        title: 'Roller',
        url: '/roles',
        icon: 'bi-clipboard2-check'
    }
]